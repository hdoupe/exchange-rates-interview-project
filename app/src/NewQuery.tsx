import { Button, Card } from "react-bootstrap";
import { useForm } from "react-hook-form";
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import api from "./api";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";


const schema = yup.object({
  name: yup.string().required(),
  countryCurrency: yup.string().required(),
  startDate: yup.date().required(),
  endDate: yup.date().nullable().transform((curr, orig) => orig === '' ? null : curr).min(yup.ref('startDate')),
}).required();
type FormData = yup.InferType<typeof schema>;

export const NewQuery = () => {
  const [countryCurrencies, setCountryCurrencies] = useState<string[]>([]);
  const navigate = useNavigate();
  const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
    resolver: yupResolver(schema)
  });
  const onSubmit = async (data: FormData) => {
    const response = await api.ExchangeRates.post(data);
    navigate(`/${response.data.id}`)
  };

  useEffect(() => {
    const fetchData = async () => {
      const response = await api.CountryCurrencies.list();
      setCountryCurrencies(response.data.map(countryCurrency => countryCurrency.name))
    }
    fetchData();
  }, []);
  return (
    <Card className="p-3 border-0">
      <Card.Title>New Query</Card.Title>
      <Card.Body>
        <form onSubmit={handleSubmit(onSubmit)}>

          <input placeholder="Query Name" {...register("name")} />
          <p className="text-danger">{errors.name?.message}</p>

          <p>Country-Currency</p>
          <input placeholder="Mexico-Peso" list="country-currencies" {...register('countryCurrency')} />
          <datalist id="country-currencies">
              {countryCurrencies.map(country_currency => (<option key={country_currency} value={country_currency} />))}
          </datalist>
          <p className="text-danger">{errors.countryCurrency?.message}</p>

          <p>Start Date</p>
          <input type="date" {...register("startDate")} />
          <p className="text-danger">{errors.startDate?.message}</p>

          <p>End Date</p>
          <input type="date" {...register("endDate")} />
          <p className="text-danger">{errors.endDate?.message}</p>

          <Button variant="primary" type="submit">Get Exchange Rates</Button>
        </form>
        </Card.Body>
    </Card>
  );
}
