import {useEffect, useState} from "react";
import {deleteOrderRequest} from "./ApiHelper/orderService.jsx"

import {fetchProductData} from "./ApiHelper/productService.jsx"
import {ApiHelper} from "./ApiHelper.jsx";

const OrderedProduct = ({ productId, cartId, quantity}) => {
    // Basic inline styles for quick demonstration
    const styles = {
        container: {
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            padding: '16px',
            border: '1px solid #ddd',
            borderRadius: '8px',
            maxWidth: '300px',
            margin: '10px auto'
        },
        productName: {
            marginBottom: '12px',
            fontSize: '1.2rem',
            color: '#333'
        },
        quantityControl: {
            display: 'flex',
            alignItems: 'center',
            gap: '12px'
        },
        button: {
            width: '36px',
            height: '36px',
            borderRadius: '50%',
            border: '1px solid #ccc',
            backgroundColor: '#f8f9fa',
            fontSize: '18px',
            cursor: 'pointer',
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center'
        },
        decrementButton: {
            borderRight: 'none',
            borderRadius: '50% 0 0 50%'
        },
        incrementButton: {
            borderLeft: 'none',
            borderRadius: '0 50% 50% 0'
        },
        quantityDisplay: {
            minWidth: '40px',
            textAlign: 'center',
            fontSize: '1.1rem',
            fontWeight: '500'
        }
    };

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [product, setProduct] = useState({});
    const apiHelper = new ApiHelper();

    const removeFromCart = async () => {
        setLoading(true);
        setError(null);
        try {
            await deleteOrderRequest(cartId, productId);
            window.location.reload();
        } catch (error) {
            setError(error.message);
            console.error('Error updating user:', error);
        } finally {
            setLoading(false);
        }
    }
    const fetchProduct = async () => {
        try {
            const data = await fetchProductData(productId);
            setProduct(data);
        } catch (e) {
            setError(e);
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        fetchProduct();
    }, [productId]);

    if (loading) return <div>Loading data...</div>;
    if (error) return <div>Error: {error.message}</div>;

    return (
        <div style={styles.container}>
            <h3 style={styles.productName}>{product.name}</h3>
            <h3 style={styles.productName}>{quantity}шт.</h3>
            
            <br/>
            <button style={{backgroundColor:"red"}} onClick={removeFromCart}>Удалить</button>
        </div>
    );
}
export default OrderedProduct;