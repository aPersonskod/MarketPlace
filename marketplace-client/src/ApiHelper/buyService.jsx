import {fetchUserData} from './userService';
import {throwHandledException} from './errorHandler.jsx';
import {handleUnauthorizedApi, getAccessToken} from './authService.jsx';

const buyHost = import.meta.env.VITE_BUY_APP_HOST;
const buyPort = import.meta.env.VITE_BUY_APP_PORT;

export const buyServiceBaseAddress = `${buyHost}:${buyPort}/api/buy-service`;

export const fetchBuyReports = async () => {
    try {
        let token = getAccessToken();
        let query = `${buyServiceBaseAddress}/get-reports-by-userid`;
        let options = {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        }
        const response = await handleUnauthorizedApi(query, options);
        if (response.ok){
            return await response.json();
        }
        await throwHandledException(response);
    } catch (e) {
        console.error("Failed to fetch cart data:", e);
        throw e; 
    }
};