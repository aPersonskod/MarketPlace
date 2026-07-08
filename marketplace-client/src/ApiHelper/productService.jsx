import {throwHandledException} from './errorHandler.jsx';

const productHost = import.meta.env.VITE_PRODUCT_APP_HOST;
const productPort = import.meta.env.VITE_PRODUCT_APP_PORT;

export const productServiceBaseAddress = `${productHost}:${productPort}/api/product-service`;

export const fetchProductsRequest = async () => {
    try{
        const response = await fetch(`${productServiceBaseAddress}/get-all`);
        if (response.ok) return await response.json();
        await throwHandledException(response);
    } catch(e){
        console.error("Failed to get product:", e);
        throw e; 
    }
};

export const fetchProductData = async (productId) => {
    try{
        const response = await fetch(`${productServiceBaseAddress}/${productId}`);
        if (response.ok) return await response.json();
        await throwHandledException(response);
    } catch(e){
        console.error("Failed to get product:", e);
        throw e; 
    }
};