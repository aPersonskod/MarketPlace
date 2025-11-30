import '../App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import Products from "../Products.jsx";
import ProductCart from "../ProductCart.jsx";
import Button from "react-bootstrap/Button";
import { useNavigate } from 'react-router';

const ProductCatalogPage = () => {
    const navigate = useNavigate();
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
                        <ProductCart/>
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