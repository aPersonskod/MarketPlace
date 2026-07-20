import {Table, Pagination, Container} from 'react-bootstrap';
import {useEffect, useState} from "react";
import {fetchBuyReports} from "../ApiHelper/buyService.jsx";

const PurchasesPage = () => {
    const [buyActions, setBuyActions] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    // State configuration
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 6;
    // Pagination logic calculations
    const indexOfLastItem = currentPage * itemsPerPage;
    const indexOfFirstItem = indexOfLastItem - itemsPerPage;
    const totalPages = Math.ceil(buyActions.recordsCount / itemsPerPage);

    // Pagination item rendering loop
    const renderPaginationItems = () => {
        let paginationItems = [];
        for (let number = 1; number <= totalPages; number++) {
            paginationItems.push(
            <Pagination.Item 
                key={number} 
                active={number === currentPage}
                onClick={() => setCurrentPage(number)}
            >
                {number}
            </Pagination.Item>
            );
        }
        return paginationItems;
    };

    const fetchBuyActions = async () => {
        try {
            let response = await fetchBuyReports(currentPage, itemsPerPage);
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
    }, [currentPage]);


    if (loading) return <div>Loading data...</div>;
    if (error) return <div>Error: {error.message}</div>;

    if (!buyActions.reports.length) {
        return (
        <Container className="mt-4">
            <p className="text-center">Покупок еще нет !!!</p>
        </Container>
        );
    }
    
    return(
        <>
        <Container className="mt-4">
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
                {buyActions.reports.map((buyAction, index) => (
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

            {/* Pagination Alignment and Controller */}
            <div className="d-flex justify-content-center mt-3">
                <Pagination>
                    <Pagination.First 
                        onClick={() => setCurrentPage(1)} 
                        disabled={currentPage === 1} 
                    />
                    <Pagination.Prev 
                        onClick={() => setCurrentPage((prev) => Math.max(prev - 1, 1))} 
                        disabled={currentPage === 1} 
                    />
                    
                    {renderPaginationItems()}
                    
                    <Pagination.Next 
                        onClick={() => setCurrentPage((prev) => Math.min(prev + 1, totalPages))} 
                        disabled={currentPage === totalPages} 
                    />
                    <Pagination.Last 
                        onClick={() => setCurrentPage(totalPages)} 
                        disabled={currentPage === totalPages} 
                    />
                </Pagination>
            </div>
        </Container>
        </>
    );
}

export default PurchasesPage;