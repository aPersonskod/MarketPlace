import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import {createBrowserRouter, RouterProvider} from "react-router";
import './index.css'
import App from './App.jsx'
import AuthPage from './Pages/AuthPage.jsx';
import PurchasesPage from "./Pages/PurchasesPage.jsx";
import ProductCatalogPage from "./Pages/ProductCatalogPage.jsx";
import ConfirmationPage from "./Pages/ConfirmationPage.jsx";

const router = createBrowserRouter([
    { path: '/auth', Component: AuthPage },
    { path: '/', Component: App, children: [
        { path: '', Component: ProductCatalogPage },
        { path: 'confirmation', Component: ConfirmationPage },
        { path: 'purchases', Component: PurchasesPage }
    ] }
]);

createRoot(document.getElementById('root')).render(
    <RouterProvider router={router}/>
)