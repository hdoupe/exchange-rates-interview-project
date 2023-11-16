import { Card, Col, Row } from "react-bootstrap";
import formatDate from "./formatDate";
import { ExchangeRatesQuery } from "./types";

interface Props {
  queries: ExchangeRatesQuery[];
}

export const Sidebar = ({ queries }: Props) => {
  if (!queries?.length) {
    return (
      <Card className="p-3 border-0">
      <Row>
        <Col>
          No queries run yet.
        </Col>
      </Row>
      </Card>
    );
  }
  return (
    <Card className="p-3 border-0">
      {queries.map(query => (
        <Row>
          <Col>
            <a href={`${query.id}`}><p className="font-weight-bold">{query.name} - {query.countryCurrency}</p></a>
            <p className="font-weight-light">
              {formatDate(query.startDate)} - { formatDate(query.endDate) || 'Today' }
            </p>
          </Col>
        </Row>
      ))}
    </Card>);
}