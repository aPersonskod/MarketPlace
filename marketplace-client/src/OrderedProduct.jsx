import {useState} from "react";

const OrderedProduct = ({ productName, orderId, quantity}) => {
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
    const [success, setSuccess] = useState(false);

    const removeFromCart = async () => {
        setLoading(true);
        setError(null);
        setSuccess(false);
        try {
            //let id = `${productId}`;
            let query = `https://localhost:7002/ShoppingCart?productId=${orderId}`;
            const response = await fetch(query, {
                method: 'DELETE',
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

    return (
        <div style={styles.container}>
            <h3 style={styles.productName}>{productName}</h3>
            <h3 style={styles.productName}>{quantity}шт.</h3>
            
            <br/>
            <button style={{backgroundColor:"red"}} onClick={removeFromCart}>Удалить</button>
        </div>
    );
}
export default OrderedProduct;