import '../App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import Products from "../Products.jsx";
import ProductCart from "../ProductCart.jsx";
import Button from "react-bootstrap/Button";
import { useNavigate } from 'react-router';
import {useEffect, useState} from "react";

const ProductCatalogPage = () => {
    const navigate = useNavigate();
    const [cart, setCart] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchCartData = async () => {
        try {
            let userId = localStorage.getItem('marketplace-user-id');
            const response = await fetch(`https://localhost:7002/ShoppingCart/${userId}`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const result = await response.json();
            setCart(result);
        } catch (err) {
            setError(err);
        } finally {
            setLoading(false);
        }
    };
    
    useEffect(() => {
        fetchCartData();
    }, []);
    const handleConfirmation = () => {
        navigate('confirmation');
    }
    
    return (
        <>
            <div className='row gy-2'>
                <div className='col col-xs-12 col-sm-12 col-lg-6'>
                    <p className='fs24'>Продукты:</p>
                    <div className='divStyle xsDivStyle mdDivStyle'>
                        <Products/>
                    </div>
                </div>
                <div className='col col-xs-12 col-sm-12 col-lg-6'>
                    <p className='fs24'>Корзина:</p>
                    <div className='divStyle xsDivStyle mdDivStyle'>
                        <ProductCart cart={cart} loading={loading} error={error}/>
                    </div>
                </div>
                <div className='col col-xs-12 col-sm-12'>
                    <Button onClick={handleConfirmation}>Подтвердить заказ</Button>
                </div>
            </div>
        </>
    );
}
export default ProductCatalogPage;