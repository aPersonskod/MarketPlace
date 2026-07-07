import {getAccessToken} from './userService';

const orderHost = import.meta.env.VITE_CART_APP_HOST;
const orderPort = import.meta.env.VITE_CART_APP_PORT;

export const cartServiceBaseAddress = `${orderHost}:${orderPort}/api/cart-service`;

export const deleteOrderRequest = async (cartId, productId) => {
    try{
        let deleteOrderDto = {
            cartId: cartId,
            orderedProductId: productId
        };
        let token = getAccessToken();
        let query = `${cartServiceBaseAddress}/delete-order`;
        const response = await fetch(query, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization' : `Bearer ${token}`
            },
            body: JSON.stringify(deleteOrderDto),
        });
        if (!response.status === 204) {
            let myLocalError = await response.json();
            throw new Error(`${myLocalError.error}`);
        }
        console.log("Order was deleted");
    } catch(e) {
        console.error("Failed to delete order:", e);
        throw e; 
    }
};