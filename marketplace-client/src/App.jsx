import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import Header from "./Header.jsx";
import Products from "./Products.jsx";
import ProductCart from "./ProductCart.jsx";

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
        <div className='container'>
            <Header/>
            <br/>
            <div className='row gy-5'>
                <div className='col col-xs-12 col-sm-12 col-lg-6'>
                    <div className='divStyle xsDivStyle mdDivStyle'>
                        <Products/>
                    </div>
                </div>
                <div className='col col-xs-12 col-sm-12 col-lg-6'>
                    <div className='divStyle xsDivStyle mdDivStyle'>
                        <ProductCart/>
                    </div>
                </div>
            </div>
        </div>
    </>
  )
}

export default App
