import { useState } from 'react'
import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import Header from "./Header.jsx";
import {Outlet} from "react-router";

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
        <div className='container'>
            <Header/>
            <Outlet/>
        </div>
    </>
  )
}

export default App
