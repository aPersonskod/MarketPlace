import Table from 'react-bootstrap/Table';
import {useEffect, useState} from "react";
import {fetchBuyReports} from "../ApiHelper/buyService.jsx";

const PurchasesPage = () => {
    const [buyActions, setBuyActions] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchBuyActions = async () => {
        try {
            let response = await fetchBuyReports();
            setBuyActions(response);
        } catch (err) {
            setError(err);
        } finally {
            setLoading(false);
        }
    };
    
    const getProducts = (orders) => {
        return (
            <ul>
                {orders.map(o => (
                    <li key={o.product.id}>{o.product.name}, цена:{o.product.cost}, {o.quantity}шт.</li>
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
            <Table responsive>
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
                        <td key={index+'01'}>{index+1}</td>
                        <td key={index+'02'}>{buyAction.cart.user.name}</td>
                        <td key={index+'03'}>{buyAction.cart.address}</td>
                        <td key={index+'04'}>{getProducts(buyAction.cart.orders)}</td>
                        <td key={index+'05'}>{buyAction.cart.amountToPay}</td>
                        <td key={index+'06'}>{getFormatedDate(buyAction.saleDate)}</td>
                    </tr>
                ))}
                </tbody>
            </Table>
        </>
    );
}

export default PurchasesPage;