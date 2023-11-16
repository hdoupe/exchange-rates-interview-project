import { Card, Col, Row } from 'react-bootstrap';

const ErrorComponent = () => {
  return (
    <Row className="w-100 h-100 justify-content-center">
      <Col className="col-auto">
        <Card>
            <Card.Title>Oops, there was an error. Please try refreshing the page.</Card.Title>
        </Card>
      </Col>
    </Row>
  );
}

export default ErrorComponent;
