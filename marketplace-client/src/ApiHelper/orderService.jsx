import {getAccessToken} from './userService';

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
        if (!response.ok) {
            let myLocalError = await response.json();
            throw new Error(`${myLocalError.error}`);
        }
        return await response.json();
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
        const response = await fetch(query, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization' : `Bearer ${token}`
            },
            body: JSON.stringify(createOrderDto)
        });
        if (!response.ok) {
            //throw new Error(`HTTP error! status: ${response.status}`);
            let myLocalError = await response.json();
            throw new Error(`${myLocalError.error}`);
        }
        return await response.json();
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