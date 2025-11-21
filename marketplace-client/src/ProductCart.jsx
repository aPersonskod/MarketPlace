import {useEffect, useState} from "react";
import OrderedProduct from "./OrderedProduct.jsx";
import ProductQuantitySelector from "./ProductQuantitySelector.jsx";

function ProductCart() {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch('https://localhost:7002/ShoppingCart');
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const result = await response.json();
                setData(result);
            } catch (err) {
                setError(err);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []); // Empty dependency array ensures this runs only once on mount

    if (loading) return <div>Loading data...</div>;
    if (error) return <div>Error: {error.message}</div>;
    
    return(
        <>
            <div className='d-flex'>
                <div style={{margin: '5px', overflowY: 'auto'}}>
                    <p className='fs24'>Корзина:</p>
                    <div className='d-flex flex-wrap'>
                        {data.orders.map((item, index) => (
                            <OrderedProduct key={index} orderId={item.id} productName={item.orderedProduct.name}
                                            quantity={item.quantity}/>
                        ))}
                    </div>
                </div>
            </div>
        </>
    );
}

export default ProductCart;