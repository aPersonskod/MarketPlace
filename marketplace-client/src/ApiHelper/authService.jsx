import {throwHandledException} from './errorHandler.jsx';

const userHost = import.meta.env.VITE_USER_APP_HOST;
const userPort = import.meta.env.VITE_USER_APP_PORT;
const accesToken = 'uToken';
const refreshToken = 'rToken';

export const userServiceBaseAddress = `${userHost}:${userPort}/api/user-service`;

export const getAccessToken = () => {
    return localStorage.getItem(accesToken);
}

export const getRefreshToken = () => {
    return localStorage.getItem(refreshToken);
}

export const removeToken = () => {
    localStorage.removeItem(accesToken);
    localStorage.removeItem(refreshToken);
}

export const setToken = (access_token, refresh_token) => {
    localStorage.setItem(accesToken, access_token);
    localStorage.setItem(refreshToken, refresh_token);
}

export const authRequest = async (email, password) => {
    try{
        let userCredentials = {
            email: email,
            password: password
        };
        const response = await fetch(`${userServiceBaseAddress}/login`, {
            method: 'POST',
            headers: {
                'accept': '*/*',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userCredentials),
        });
        if (response.ok){
            const token = await response.json();
            setToken(token.access_token, token.refresh_token);
        }
        await throwHandledException(response);
    } catch(e){
        console.error("Failed to auth:", e);
        throw e; 
    }
};

export const refreshRequest = async () => {
    try{
        let tokenData = {
            access_token: getAccessToken(),
            refresh_token: getRefreshToken()
        };
        const response = await fetch(`${userServiceBaseAddress}/refresh`, {
            method: 'POST',
            headers: {
                'accept': '*/*',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(tokenData),
        });
        if (response.ok){
            const token = await response.json();
            setToken(token.access_token, token.refresh_token);
        }
        await throwHandledException(response);
    } catch(e){
        console.error("Failed to refresh token:", e);
        throw e; 
    }
}

export const handleUnauthorizedApi = async (query, options) => {
    let response = await fetch(query, options);
    if (!getRefreshToken()) return response;
    if(response.status === 401){
        await refreshRequest();
        options.headers.Authorization = `Bearer ${getAccessToken()}`;
        response = await fetch(query, options);
    }
    return response;
}