import {useState, useEffect} from "react";
import Button from "react-bootstrap/Button";
import {ApiHelper} from "./ApiHelper.jsx";
import { useNavigate } from 'react-router';

const ProductQuantitySelector = ({ productName, productCost, productId, setAmmountToPay, cart, refreshCartFunc, minQuantity = 0, maxQuantity = 99}) => {
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
    
    const [counter, setCounter] = useState(0);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [success, setSuccess] = useState(false);
    const apiHelper = new ApiHelper();
    const navigate = useNavigate();

    useEffect(() => {
        fetchOrders();
    }, []);

    const fetchOrders = async () => {
        try {
            if(cart === null) return;
            const response = await fetch(`${apiHelper.cartServiceBaseAddress}/get-cart-orders/${cart.id}`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const orders = await response.json();
            let currentQuantity = getInitialQuantity(productId, orders);
            setCounter(currentQuantity);
        } catch (err) {
            setError(err);
        } finally {
            setLoading(false);
        }
    }

    const getInitialQuantity = (productId, orders) => {
        if (orders.length === 0) return 0;
        let orderedProduct = orders.find(p => p.orderedProductId === productId);
        let result = orderedProduct === undefined ? 0 : orderedProduct.quantity;
        return result;
    }


    const increment = async () => {
        if (counter < maxQuantity) {
            let tempQuantity = counter + 1;
            await updateCart(tempQuantity);
        }
    };

    const decrement = async () => {
        if (counter > minQuantity) {
            let tempQuantity = counter - 1;
            await updateCart(tempQuantity);
        }
    };

    const addToShoppingCartBtnHandler = async () => {
        let user = await apiHelper.getUser();
        if(user === null) {
            navigate('auth');
            return;
        }
        setLoading(true);
        setError(null);
        setSuccess(false);
        let tempQuantity = 1;
        await updateCart(tempQuantity, user);
    }

    const updateCart = async (amount, user) => {
        setCounter(amount);
        await addProductToCart(amount, user);
        refreshCartFunc();
        //await refreshAnotherComponents();
    }

    const addProductToCart = async (amount, user) => {
        setLoading(true);
        setError(null);
        setSuccess(false);
        try {
            let token = apiHelper.getAccessToken();
            let order = {
                cartId: cart.id,
                orderedProductId: productId,
                quantity: amount
            };
            let query = `${apiHelper.cartServiceBaseAddress}/add-order`;
            const response = await fetch(query, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization' : `Bearer ${token}`
                },
                body: JSON.stringify(order)
            });

            if (!response.ok) {
                //throw new Error(`HTTP error! status: ${response.status}`);
                let myLocalError = await response.json();
                throw new Error(`${myLocalError.error}`);
                alert(`ADD HTTP error: ${myLocalError.error}`);
            }

            const data = await response.json();
            console.log('Update successful:', data);
            setSuccess(true);
            // Optionally, update local state or re-fetch data after successful update
        } catch (error) {
            setError(error.message);
            console.error('Error updating user:', error);
        } finally {
            setLoading(false);
        }
    }

    //if (loading) return <div>Loading data...</div>;
    if (error) return <div>Error: {error.message}</div>;

    return (
        <div style={styles.container}>
            <h3 style={styles.productName}>{productName}</h3>

            {
            counter !== 0 &&    
            <div style={styles.quantityControl}>
                <button
                    onClick={decrement}
                    disabled={counter <= minQuantity}
                    style={{
                        ...styles.button,
                        ...styles.decrementButton,
                        opacity: counter <= minQuantity ? 0.5 : 1
                    }}
                >
                    -
                </button>

                <span style={styles.quantityDisplay}>{counter}</span>

                <button
                    onClick={increment}
                    disabled={counter >= maxQuantity}
                    style={{
                        ...styles.button,
                        ...styles.incrementButton,
                        opacity: counter >= maxQuantity ? 0.5 : 1
                    }}
                >
                    +
                </button>
            </div>
            }
            <br/>
            <p className='fs21'>Цена: {productCost}</p>
            {
                counter === 0 && <Button onClick={addToShoppingCartBtnHandler}>Добавить в корзину</Button>
            }
        </div>
    );
}
export default ProductQuantitySelector;