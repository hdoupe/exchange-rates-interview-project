import moment from "moment";

const formatDate = (date?: string | Date | null) => {
  if (!date) return null;
  return moment(date).format('YYYY-MM-DD');
}

export default formatDate;
