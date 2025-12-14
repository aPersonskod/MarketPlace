import 'bootstrap/dist/css/bootstrap.min.css';
import Button from "react-bootstrap/Button";
import Form from 'react-bootstrap/Form';
import {useState, useEffect} from "react";
import { useNavigate } from 'react-router';
import Products from "../Products.jsx";
import ProductCart from "../ProductCart.jsx";
import Places from "../Places.jsx";
import SumToPay from "../SumToPay.jsx";
import {ApiHelper} from "../ApiHelper.jsx";
const ConfirmationPage = () => {
    const navigate = useNavigate();
    const [selectedPlaceId, setSelectedPlaceId] = useState('');
    const [cart, setCart] = useState(null);
    const [isCartConfirmed, setIsCartConfirmed] = useState(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const apiHelper = new ApiHelper();

    const fetchCartData = async () => {
        try {
            let userId = localStorage.getItem('marketplace-user-id');
            const response = await fetch(`${apiHelper.shoppingCartBaseAddress}/GetCart?userId=${userId}`);
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
    
    const confirmCart = async () => {
        try {
            let userId = localStorage.getItem('marketplace-user-id');
            let query = `${apiHelper.shoppingCartBaseAddress}/ConfirmCart?userId=${userId}&placeId=${selectedPlaceId}`;
            const response = await fetch(query, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                }//,
                //body: JSON.stringify(requestBody),
            });

            if (!response.ok) {
                let myLocalError = await response.json();
                throw new Error(`${myLocalError.message}`);
                //alert(`HTTP error! status: ${response.status}`);
            }
            let result = await response.json();
            setCart(result);
            console.log('Заказ подтвержден !!!');
            return {
                isConfirmed: true,
                cart: result
            };
        } catch (err) {
            console.error('Error confirm cart!', err);
            alert(err);
        }
    }
    
    const buyCart = async (cartParam) => {
        try {
            let query = `${apiHelper.buyActionsBaseAddress}/BuyCart`;
            const response = await fetch(query, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(cartParam),
            });

            if (!response.ok) {
                let myLocalError = await response.json();
                throw new Error(myLocalError.message);
                //alert(`HTTP error! status: ${response.status}`);
            }
            alert("Заказ куплен !!!")
        } catch (err) {
            console.error('Error buy cart!', err);
            alert(err);
        }
    }

    useEffect(() => {
        fetchCartData();
    }, []);

    const handlePlaceChange = (event) => {
        setSelectedPlaceId(event.target.value);
    };
    const handleBack = () => {
        navigate('/');
    }
    const handleBuy = async () => {
        let res = await confirmCart();
        if (res?.isConfirmed) {
            console.log('Покупка осуществлена !!!');
            alert('Покупка осуществлена !!!');
            //await buyCart(res.cart);
            navigate('/');
        }
    }

    if (loading) return <div>Loading data...</div>;
    if (error) return <div>Error: {error.message}</div>;
    
    return (
      <>
          <div className='row gy-2'>
              <div className='col col-xs-12 col-sm-12'>
                  <p className='fs24'>Место доставки:</p>
                  <div className='divStyleSmall'>
                    <Places selectedPlace={selectedPlaceId} setSelectedPlace={setSelectedPlaceId} onSelectChangeEvent={handlePlaceChange} />
                  </div>
              </div>
              <div className='col col-xs-12 col-sm-12'>
                  <p className='fs24'>Корзина:</p>
                  <div className='divStyleHalf xsDivStyle mdDivStyle'>
                      <ProductCart cart={cart} />
                  </div>
              </div>
              <div className='col col-xs-12 col-sm-12'>
                  <SumToPay cart={cart} loading={loading} error={error}/>
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