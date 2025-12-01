import Table from 'react-bootstrap/Table';
import {useEffect, useState} from "react";

const PurchasesPage = () => {
    const [buyActions, setBuyActions] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchBuyActions = async () => {
        try {
            const response = await fetch(`https://localhost:7003/BuyActions`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const result = await response.json();
            setBuyActions(result);
        } catch (err) {
            setError(err);
        } finally {
            setLoading(false);
        }
    };
    
    const getProducts = (orders) => {
        return (
            <ul>
                {orders.map(order => (
                    <li key={order.id}>{order.orderedProduct.name}, цена:{order.orderedProduct.cost}, {order.quantity}шт.</li>
                ))}
            </ul>
        );
    }
    
    const getFormatedDate = (date) => {
        const now = new Date(date);

        const year = now.getFullYear();
        const month = (now.getMonth() + 1).toString().padStart(2, '0'); // Months are 0-indexed
        const day = now.getDate().toString().padStart(2, '0');
        const hours = now.getHours().toString().padStart(2, '0');
        const minutes = now.getMinutes().toString().padStart(2, '0');
        const seconds = now.getSeconds().toString().padStart(2, '0');

        return `${day}.${month}.${year} ${hours}:${minutes}:${seconds}`;
    }

    useEffect(() => {
        fetchBuyActions();
    }, []);


    if (loading) return <div>Loading data...</div>;
    if (error) return <div>Error: {error.message}</div>;
    
    return(
        <>
            <p className='fs24'>История покупок</p>
            <Table responsive="sm">
                <thead>
                <tr>
                    <th>#</th>
                    <th>Имя</th>
                    <th>Куда заказал</th>
                    <th>Покупки</th>
                    <th>Общая сумма заказа</th>
                    <th>Дата</th>
                </tr>
                </thead>
                <tbody>
                {buyActions.map((buyAction, index) => (
                    <tr key={index+'tr'}>
                        <td key={index+'td'}>{index+1}</td>
                        <td key={index+buyAction.cart.id}>{buyAction.cart.user.name}</td>
                        <td key={index+buyAction.cart.place.id}>{buyAction.cart.place.address}</td>
                        <td key={index+index}>{getProducts(buyAction.cart.orders)}</td>
                        <td key={index+buyAction.cart.amount_to_pay}>{buyAction.cart.amount_to_pay}</td>
                        <td key={index+buyAction.saleDate}>{getFormatedDate(buyAction.saleDate)}</td>
                    </tr>
                ))}
{/*                <tr>
                    <td>1</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                </tr>
                <tr>
                    <td>2</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                </tr>
                <tr>
                    <td>3</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                    <td>Table cell</td>
                </tr>*/}
                </tbody>
            </Table>
        </>
    );
}

export default PurchasesPage;