import {useEffect, useState} from "react";
import Form from "react-bootstrap/Form";

const Places = ({selectedPlace, setSelectedPlace, onSelectChangeEvent}) => {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch(`https://localhost:7002/ShoppingCart/GetPlaces`);
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const result = await response.json();
                setData(result);
                setSelectedPlace(result[0].id);
            } catch (err) {
                setError(err);
            } finally {
                setLoading(false);
            }
        };
        
        fetchData();
    }, []);

    if (loading) return <div>Loading data...</div>;
    if (error) return <div>Error: {error.message}</div>;
    
    return(
      <>
          <Form.Select onChange={onSelectChangeEvent} value={selectedPlace} aria-label="Default select example" style={{marginTop: '10px'}}>
              {data.map(item => (
                  <option key={item.id} value={item.id}>{item.address}</option>
              ))}
          </Form.Select>
      </>
    );
}

export default Places;