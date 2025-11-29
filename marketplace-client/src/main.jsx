import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import {createBrowserRouter, RouterProvider} from "react-router";
import './index.css'
import App from './App.jsx'
import Authorization from "./Authorization.jsx";
import Registration from "./Registration.jsx";
import Purchases from "./Purchases.jsx";

const router = createBrowserRouter([
    { path: '/', Component: App },
    { path: '/authorize', Component: Authorization },
    { path: '/registration', Component: Registration },
    { path: '/purchases', Component: Purchases }
]);

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <RouterProvider router={router}/>
  </StrictMode>,
)
