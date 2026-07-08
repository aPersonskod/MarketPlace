import {throwHandledException} from './errorHandler.jsx';

const placeHost = import.meta.env.VITE_CART_APP_HOST;
const placePort = import.meta.env.VITE_CART_APP_PORT;

export const cartServiceBaseAddress = `${placeHost}:${placePort}/api/cart-service`;

export const fetchPlaces = async () => {
    try {
        const response = await fetch(`${cartServiceBaseAddress}/get-places`);
        if (response.ok) return await response.json();
        await throwHandledException(response);
    } catch(e) {
        console.error("Failed to fetch places:", e);
        throw e; 
    }
};