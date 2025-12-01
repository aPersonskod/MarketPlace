const SumToPay = ({cart, loading, error}) => {
    if (loading) return <div>Loading data...</div>;
    if (error) return <div>Error: {error.message}</div>;
    
    return (
        <>
            <p className='fs24'>Сумма к оплате: {cart.amount_to_pay}</p>
        </>
    );
}

export default SumToPay;