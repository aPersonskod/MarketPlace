import { useState, useEffect } from 'react';
import ProductQuantitySelector from "./ProductQuantitySelector.jsx";

function Products() {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch('https://localhost:7001/ProductCatalog');
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
                <div style={{margin:'5px'}}>
                    <div className='d-flex flex-wrap'>
                        {data.map((item, index) => (
                            <ProductQuantitySelector productId={item.id} productName={item.name} productCost={item.cost} key={index} />
                        ))}
                    </div>
                </div>
            </div>
        </>
    );
}

export default Products;