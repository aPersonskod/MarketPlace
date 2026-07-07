import {getAccessToken, getUser} from './userService';

const buyHost = import.meta.env.VITE_BUY_APP_HOST;
const buyPort = import.meta.env.VITE_BUY_APP_PORT;

export const buyServiceBaseAddress = `${buyHost}:${buyPort}/api/buy-service`;

export const fetchBuyReports = async () => {
    try {
        let user = await getUser();
        if (user === null) setBuyActions([]);
        let token = getAccessToken();
        let query = `${buyServiceBaseAddress}/get-reports-by-userid`;
        let options = {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        }
        const response = await fetch(query, options);
        if (!response.ok) {
            let myLocalError = await response.json();
            throw new Error(`${myLocalError.error}`);
        }
        return await response.json();
    } catch (e) {
        console.error("Failed to fetch cart data:", e);
        throw e; 
    }
};