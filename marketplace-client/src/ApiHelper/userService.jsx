import {throwHandledException} from './errorHandler.jsx';
import {handleUnauthorizedApi, getAccessToken} from './authService.jsx';

const userHost = import.meta.env.VITE_USER_APP_HOST;
const userPort = import.meta.env.VITE_USER_APP_PORT;

export const userServiceBaseAddress = `${userHost}:${userPort}/api/user-service`;

export const walletReplenishmentRequest = async () => {
    try{
        let moneyDto = {
            money: 300
        };
        let query = `${userServiceBaseAddress}/top-up-money`;
        let options = {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${getAccessToken()}`
            },
            body: JSON.stringify(moneyDto),
        };
        const response = await handleUnauthorizedApi(query, options);
        if (response.ok) return await response.json();
        await throwHandledException(response);
    } catch(e){
        console.error("Failed to top-up money:", e);
        throw e; 
    }
};

export const createUser = async (name, email, password) => {
    let createUserDto = {
        name: name,
        email: email,
        password: password,
        wallet: 100,
        role: 'user'
    };
    let query = `${userServiceBaseAddress}`;
    let options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(createUserDto)
    };
    const response = await fetch(query, options);
    if (response.ok) return await response.json();
    await throwHandledException(response);
};

export const fetchUserData = async () => {
    try{
        let token = getAccessToken();
        if (token === null) return null;
        let query = `${userServiceBaseAddress}`;
        let options = {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        };
        const response = await handleUnauthorizedApi(query, options);
        if (response.ok) return await response.json();
        await throwHandledException(response);
    }
    catch(e){
        console.error("Network or CORS error:", e);
        return null;
    }
}

