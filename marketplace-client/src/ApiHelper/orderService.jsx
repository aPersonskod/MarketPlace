import {handleUnauthorizedApi, getAccessToken} from './authService.jsx';
import {throwHandledException} from './errorHandler.jsx';

const orderHost = import.meta.env.VITE_CART_APP_HOST;
const orderPort = import.meta.env.VITE_CART_APP_PORT;

export const cartServiceBaseAddress = `${orderHost}:${orderPort}/api/cart-service`;

export const getCartOrders = async (cartId) => {
    try{
        let token = getAccessToken();
        let url = `${cartServiceBaseAddress}/get-cart-orders/${cartId}`;
        let options = {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`,
            },
        };
        const response = await fetch(url, options);
        if (response.ok) return await response.json();
        await throwHandledException(response);
    } catch(e){
        console.error("Failed to get orders:", e);
        throw e; 
    }
};

export const createOrderRequest = async (cartId, productId, quantity) => {
    try{
        let createOrderDto = {
            cartId: cartId,
            orderedProductId: productId,
            quantity: quantity
        };
        let token = getAccessToken();
        let query = `${cartServiceBaseAddress}/add-order`;
        let options = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization' : `Bearer ${token}`
            },
            body: JSON.stringify(createOrderDto)
        };
        const response = await handleUnauthorizedApi(query, options);
        if (response.ok) return await response.json();
        await throwHandledException(response);
    } catch(e){
        console.error("Failed to add order:", e);
        throw e; 
    }
};

export const deleteOrderRequest = async (cartId, productId) => {
    try{
        let deleteOrderDto = {
            cartId: cartId,
            orderedProductId: productId
        };
        let token = getAccessToken();
        let query = `${cartServiceBaseAddress}/delete-order`;
        let options = {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization' : `Bearer ${token}`
            },
            body: JSON.stringify(deleteOrderDto),
        };
        const response = await handleUnauthorizedApi(query, options);
        if (response.ok) console.log("Order was deleted");
        await throwHandledException(response);
    } catch(e) {
        console.error("Failed to delete order:", e);
        throw e; 
    }
};