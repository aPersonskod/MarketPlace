import 'bootstrap/dist/css/bootstrap.min.css';
import Button from "react-bootstrap/Button";
import Form from 'react-bootstrap/Form';
import { useNavigate } from 'react-router';
import Products from "../Products.jsx";
import ProductCart from "../ProductCart.jsx";
const ConfirmationPage = () => {
    const navigate = useNavigate();
    const handleBack = () => {
        navigate('/');
    }
    const handleBuy = () => {
        navigate('/');
    }
  return (
      <>
          <div className='row gy-2'>
              <div className='col col-xs-12 col-sm-12'>
                  <p className='fs24'>Место доставки:</p>
                  <div className='divStyleSmall'>
                      <Form.Select aria-label="Default select example" style={{marginTop: '10px'}}>
                          <option value="1">One</option>
                          <option value="2">Two</option>
                          <option value="3">Three</option>
                      </Form.Select>
                  </div>
              </div>
              <div className='col col-xs-12 col-sm-12'>
                  <p className='fs24'>Корзина:</p>
                  <div className='divStyleHalf xsDivStyle mdDivStyle'>
                      <ProductCart/>
                  </div>
              </div>
              <div className='col col-xs-12 col-sm-12'>
                  <p className='fs24'>Сумма к оплате:</p>
              </div>
              <div className='col col-xs-12 col-sm-12'>
                  <Button onClick={handleBuy} style={{marginRight: '10px'}}>Купить</Button>
                  <Button onClick={handleBack}>Назад</Button>
              </div>
          </div>
      </>
  );
}

export default ConfirmationPage;