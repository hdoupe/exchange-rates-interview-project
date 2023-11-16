import { AxiosError } from 'axios';
import { useEffect, useState } from 'react';
import { Alert, Col, Row } from 'react-bootstrap';
import { useParams } from 'react-router-dom';
import api from './api';
import { NewQuery } from './NewQuery';
import { Sidebar } from './Queries';
import { QueryDetail } from './QueryDetail';
import { ExchangeRatesQuery, QueryResult } from './types';

const App = () => {
  const [queries, setQueries] = useState<ExchangeRatesQuery[]>([]);
  const [queryResult, setQueryResult] = useState<QueryResult>();
  const [error, setError] = useState("");

  const { id } = useParams();

  useEffect(() => {
    const fetchData = async () => {
      const listResponse = await api.ExchangeRates.list();
      setQueries(listResponse.data);
      setError('');
      if (id) {
        try {
          const detailResponse = await api.ExchangeRates.get(id as unknown as number);
          setQueryResult(detailResponse.data);
        } catch (error) {
          if (error instanceof AxiosError && error.response?.status === 404) {
            setError('This query does not exist.')
          } else {
            setError('There was an error retrieving the exchange rates for this query.')
          }
        }
      }
    }
    fetchData();
  }, [id]);
  return (
    <Row className="w-100 h-100 justify-content-center">
      <Col className="col-3">
        <Sidebar queries={queries} />
      </Col>
      <Col className="col-6">
        {error && <Alert className="m-3" variant="danger">{error}</Alert>}
        {id && queryResult && <QueryDetail queryResult={queryResult} />}
        {id && !queryResult && !error && <p className="m-3">Loading...</p>}
        {!id && <NewQuery />}
      </Col>
    </Row>
  );
}

export default App;
