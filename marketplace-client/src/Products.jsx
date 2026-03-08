import { useState, useEffect } from 'react';
import ProductQuantitySelector from "./ProductQuantitySelector.jsx";
import {ApiHelper} from "./ApiHelper.jsx";

function Products({cart, setAmmountToPay}) {
    const [products, setProducts] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const apiHelper = new ApiHelper();

    const fetchProducts = async () => {
        try {
            const response = await fetch(apiHelper.productCatalogBaseAddress);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const result = await response.json();
            setProducts(result);
        } catch (err) {
            setError(err);
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        const fetchData = async () => {
            await fetchProducts();
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
                        {products.map((product, index) => (
                            <ProductQuantitySelector key={index} productId={product.id} productName={product.name} 
                            productCost={product.cost} setAmmountToPay={setAmmountToPay} cartId={cart.id}/>
                        ))}
                    </div>
                </div>
            </div>
        </>
    );
}

export default Products;