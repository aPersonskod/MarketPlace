import OrderedProduct from "./OrderedProduct.jsx";
import {useEffect, useState} from "react";
import {getCartOrders} from "./ApiHelper/orderService.jsx"

function ProductCart({cart}) {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    
    const fetchOrdersData = async () => {
        try {
            if(cart === null) return;
            const response = await getCartOrders(cart.id);
            setOrders(response);
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
                        {!error && orders.map((item, index) => (
                            <OrderedProduct key={index} productId={item.orderedProductId} cartId={item.cartId} quantity={item.quantity}/>
                        ))}
                    </div>
                </div>
            </div>
        </>
    );
}

export default ProductCart;