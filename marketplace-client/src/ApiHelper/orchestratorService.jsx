import {getAccessToken} from './userService';

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
        const response = await fetch(query, options);
    
        if (!response.ok) {
            let myLocalError = await response.json();
            throw new Error(`${myLocalError.error}`);
            //alert(`HTTP error! status: ${response.status}`);
        }
        console.log("Succesfully submit cart !!!");
    } catch(e) {
        console.error("Failed to buy cart:", e);
        throw e; 
    }
}