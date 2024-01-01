using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class AuthenticationWrapper
{
    public static AuthState authState { get; private set; } = AuthState.NotAuthenticated;

    public static async Task<AuthState> DoAuth(int maxTries = 5)
    {
        if(authState == AuthState.Authenticated)
        {
            return authState;
        }

        if(authState == AuthState.Authenticating)
        {
            Debug.LogWarning("Already authenticating..!");
            await Authenticating();
            return authState;
        }

        await SignInAnonymouslyAsync(maxTries);

        return authState;
    }

    private static async Task<AuthState> Authenticating()
    {
        while(authState == AuthState.Authenticating || authState == AuthState.NotAuthenticated)
        {
            await Task.Delay(200);
        }

        return authState;
    }

    private static async Task SignInAnonymouslyAsync(int maxTries)
    {
        authState = AuthState.Authenticating;

        int tries = 0;

        while(authState == AuthState.Authenticating && tries < maxTries)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if(AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    authState = AuthState.Authenticated;
                    break;
                }
            }
            catch(AuthenticationException e)
            {
                Debug.LogError(e);
                authState = AuthState.Error;
            }
            catch(RequestFailedException e)
            {
                Debug.LogError(e);
                authState = AuthState.Error;
            }

            tries++;
            await Task.Delay(1000); // 1 sec
        }

        if(authState != AuthState.Authenticated)
        {
            Debug.LogWarning("Player was not signed in successfully after " + tries + " tries.");
            authState = AuthState.TimeOut;
        }
    }

}

public enum AuthState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error,
    TimeOut
}
