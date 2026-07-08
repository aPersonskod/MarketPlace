import {throwHandledException} from './errorHandler.jsx';

const userHost = import.meta.env.VITE_USER_APP_HOST;
const userPort = import.meta.env.VITE_USER_APP_PORT;

export const userServiceBaseAddress = `${userHost}:${userPort}/api/user-service`;

export const walletReplenishmentRequest = async () => {
    try{
        let moneyDto = {
            money: 300
        };
        let query = `${userServiceBaseAddress}/wallet-replenishment?money=${money}`;
        const response = await fetch(query, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${getAccessToken()}`
            },
            body: JSON.stringify(moneyDto),
        });
        if (response.ok) return await response.json();
        await throwHandledException(response);
    } catch(e){
        console.error("Failed to top-up money:", e);
        throw e; 
    }
};

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
            localStorage.setItem('uToken', token);
        }
        await throwHandledException(response);
    } catch(e){
        console.error("Failed to delete cart:", e);
        throw e; 
    }
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
        const response = await fetch(query, options);
        if (response.ok) return await response.json();
        await throwHandledException(response);
    }
    catch(e){
        console.error("Network or CORS error:", e);
        return null;
    }
}

export const getAccessToken = () => {
    return localStorage.getItem('uToken');
}

