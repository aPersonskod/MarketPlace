import OrderedProduct from "./OrderedProduct.jsx";
import {useEffect, useState} from "react";
import {ApiHelper} from "./ApiHelper.jsx";

function ProductCart({cart}) {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const apiHelper = new ApiHelper();
    
    const fetchOrdersData = async () => {
        try {
            const response = await fetch(`${apiHelper.shoppingCartBaseAddress}/GetCartOrders?cartId=${cart.id}`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const result = await response.json();
            setOrders(result);
        } catch (err) {
            setError(err);
        } finally {
            setLoading(false);
        }
    }
    
    useEffect(() => {
        fetchOrdersData();
    }, [cart]);
    
    if (loading) return <div>Loading data...</div>;
    if (error) return <div>Error: {error.message}</div>;
    
    return(
        <>
            <div className='d-flex'>
                <div style={{margin: '5px', overflowY: 'auto'}}>
                    <div className='d-flex flex-wrap'>
                        {orders.map((item, index) => (
                            <OrderedProduct key={index} productId={item.orderedProductId} quantity={item.quantity}/>
                        ))}
                    </div>
                </div>
            </div>
        </>
    );
}

export default ProductCart;