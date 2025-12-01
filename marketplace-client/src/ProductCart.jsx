import OrderedProduct from "./OrderedProduct.jsx";

function ProductCart({cart, loading, error}) {
    if (loading) return <div>Loading data...</div>;
    if (error) return <div>Error: {error.message}</div>;
    
    return(
        <>
            <div className='d-flex'>
                <div style={{margin: '5px', overflowY: 'auto'}}>
                    <div className='d-flex flex-wrap'>
                        {cart.orders.map((item, index) => (
                            <OrderedProduct key={index} productId={item.orderedProduct.id} productName={item.orderedProduct.name}
                                            quantity={item.quantity}/>
                        ))}
                    </div>
                </div>
            </div>
        </>
    );
}

export default ProductCart;