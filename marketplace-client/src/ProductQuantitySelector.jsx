import {useState} from "react";

const ProductQuantitySelector = ({ productName, productId, initialQuantity = 0, minQuantity = 0, maxQuantity = 99 }) => {
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
    
    const [quantity, setQuantity] = useState(initialQuantity);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(false);
    const increment = () => {
        if (quantity < maxQuantity) {
            setQuantity(prev => prev + 1);
        }
    };

    const decrement = () => {
        if (quantity > minQuantity) {
            setQuantity(prev => prev - 1);
        }
    };
    
    const putToShoppingCart = async () => {
        setLoading(true);
        setError(null);
        setSuccess(false);
        if (quantity >= 1) {
            try {
                //let id = `${productId}`;
                let query = `https://localhost:7002/ShoppingCart?productId=${productId}&quantity=${quantity}`;
                const response = await fetch(query, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    }//,
                    //body: JSON.stringify(requestBody),
                });

                if (!response.ok) {
                    //throw new Error(`HTTP error! status: ${response.status}`);
                    alert(`HTTP error! status: ${response.status}`);
                }

                const data = await response.json();
                console.log('Update successful:', data);
                setSuccess(true);
                window.location.reload();
                // Optionally, update local state or re-fetch data after successful update
            } catch (error) {
                setError(error.message);
                console.error('Error updating user:', error);
            } finally {
                setLoading(false);
            }
        }
    }

    return (
        <div style={styles.container}>
            <h3 style={styles.productName}>{productName}</h3>

            <div style={styles.quantityControl}>
                <button
                    onClick={decrement}
                    disabled={quantity <= minQuantity}
                    style={{
                        ...styles.button,
                        ...styles.decrementButton,
                        opacity: quantity <= minQuantity ? 0.5 : 1
                    }}
                >
                    -
                </button>

                <span style={styles.quantityDisplay}>{quantity}</span>

                <button
                    onClick={increment}
                    disabled={quantity >= maxQuantity}
                    style={{
                        ...styles.button,
                        ...styles.incrementButton,
                        opacity: quantity >= maxQuantity ? 0.5 : 1
                    }}
                >
                    +
                </button>
            </div>
            <br/>
            <button onClick={putToShoppingCart}>Добавить в корзину</button>
        </div>
    );
}
export default ProductQuantitySelector;