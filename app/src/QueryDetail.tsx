import { Button, Card, Col, Row, Table } from "react-bootstrap";
import formatDate from "./formatDate";
import { QueryResult } from "./types";

interface Props {
  queryResult: QueryResult;
}

export const QueryDetail = ({ queryResult }: Props) => {
  const { query, response } = queryResult;
  return (
    <Card className="p-3 border-0">
      <Card.Title>
        <Row className="justify-content-between">
          <Col className="col-auto">{query.name}</Col>
          <Col className="col-auto"><Button href="/" variant="primary">New Query</Button></Col>
        </Row>
      </Card.Title>
      <Card.Body>
        <p><i>Country-Currency:</i> {query.countryCurrency}</p>
        <p>
          <i>Queried Dates:</i> {formatDate(query.startDate)} - {formatDate(query.endDate) || 'Today'}
        </p>
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>Recorded Date</th>
              <th>Rate</th>
            </tr>
          </thead>
          <tbody>
            {response.data.map(exchangeRate => (
              <tr>
                <td>{formatDate(exchangeRate.record_date)}</td>
                <td>{exchangeRate.exchange_rate}</td>
              </tr>
            ))}
          </tbody>
        </Table>
      </Card.Body>
    </Card>
  );
}