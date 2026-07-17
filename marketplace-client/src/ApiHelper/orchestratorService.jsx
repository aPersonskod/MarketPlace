import {handleUnauthorizedApi, getAccessToken, refreshRequest} from './authService.jsx';
import {throwHandledException} from './errorHandler.jsx';

const orchestratorHost = import.meta.env.VITE_ORCHESTRATOR_APP_HOST;
const orchestratorPort = import.meta.env.VITE_ORCHESTRATOR_APP_PORT;

export const orchestratorServiceBaseAddress = `${orchestratorHost}:${orchestratorPort}/api/buy-actions`;

export const buyCartRequest = async (cartId, placeId) => {
    try{
        let token = getAccessToken();
        let cartSubmittedDto = {
            cartId: cartId,
            placeId: placeId
        };
        let query = `${orchestratorServiceBaseAddress}/buy-cart`;
        let options = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(cartSubmittedDto)
        }
        await refreshRequest();
        const response = await fetch(query, options);
        if (response.ok) return "Succesfully submit cart !!!";
        await throwHandledException(response);
    } catch(e) {
        console.error("Failed to buy cart:", e);
        throw e; 
    }
}