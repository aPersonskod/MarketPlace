import {getAccessToken} from './userService';
import {throwHandledException} from './errorHandler.jsx';

const cartHost = import.meta.env.VITE_CART_APP_HOST;
const cartPort = import.meta.env.VITE_CART_APP_PORT;

export const cartServiceBaseAddress = `${cartHost}:${cartPort}/api/cart-service`;

export const fetchCartData = async () => {
    try{
        let token = getAccessToken();
        let query = `${cartServiceBaseAddress}/get-cart`;
        let options = {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        };
        const response = await fetch(query, options);
        if (response.ok) return await response.json();
        await throwHandledException(response);
    } catch(e) {
        console.error("Failed to fetch cart data:", e);
        throw e; 
    }
};

export const deleteCartRequest = async (cartId) => {
    try{
        let token = getAccessToken();
        let query = `${cartServiceBaseAddress}/delete-cart/${cartId}`;
        const response = await fetch(query, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization' : `Bearer ${token}`
            }
        });
        if (response.ok) console.log("Cart was deleted")
        await throwHandledException(response);
    } catch(e){
        console.error("Failed to delete cart:", e);
        throw e; 
    }
};