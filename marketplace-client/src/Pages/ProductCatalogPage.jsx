import '../App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import Products from "../Products.jsx";
import ProductCart from "../ProductCart.jsx";
import Button from "react-bootstrap/Button";
import { useNavigate } from 'react-router';
import {useEffect, useState} from "react";
import {fetchCartData, deleteCartRequest} from "../ApiHelper/cartService.jsx"

const ProductCatalogPage = () => {
    const navigate = useNavigate();
    const [cart, setCart] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [ammountToPay, setAmmountToPay] = useState(0);

    const fetchCart = async () => {
        try {
            const response = await fetchCartData();
            setCart(response);
        } catch (err) {
            setError(err);
        } finally {
            setLoading(false);
        }
    };

    const handleDeleteCart = async () => {
        try {
            await deleteCartRequest();
            await fetchCart();
        } catch (err) {
            setError(err);
        } finally {
            setLoading(false);
        }
    }
    
    useEffect(() => {
        fetchCart();
    }, []);

    const handleConfirmation = () => {
        if(cart === null){
            console.error('Cart must not be empty !!!');
        } else {
            navigate('confirmation');
        }
    }

    if (loading) return <div>Loading data...</div>;
    if (error) return <div>Error: {error.message}</div>;
    
    return (
        <>
            <div className='row gy-2'>
                <div className='col col-xs-12 col-sm-12 col-lg-6'>
                    <p className='fs24'>Продукты:</p>
                    <div className='divStyle xsDivStyle mdDivStyle'>
                        <Products cart={cart} setAmmountToPay={setAmmountToPay} refreshCartFunc={fetchCartData}/>
                    </div>
                </div>
                <div className='col col-xs-12 col-sm-12 col-lg-6'>
                    <div className='d-flex'>
                        <p className='fs24'>Корзина:</p>
                        {cart !== null &&
                            <div>
                                <Button style={{backgroundColor:"red", marginLeft:"10px"}} onClick={handleDeleteCart}>Удалить</Button>
                            </div>
                        }
                        {cart !== null &&
                            <div className='d-flex'>
                                <p className='fs24' style={{marginLeft:"10px"}}>Сумма к оплате:</p>
                                <p className='fs24' style={{marginLeft:"10px"}}>{cart.amountToPay}</p>
                            </div>
                        }   
                    </div>
                    <div className='divStyle xsDivStyle mdDivStyle'>
                        <ProductCart key={ammountToPay} cart={cart}/>
                    </div>
                </div>
                {((cart !== null) && (cart.ammountToPay !== 0)) &&
                <div className='col col-xs-12 col-sm-12'>
                    <Button onClick={handleConfirmation}>Подтвердить заказ</Button>
                </div>
                }
            </div>
        </>
    );
}
export default ProductCatalogPage;