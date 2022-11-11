// eslint-disable-next-line @typescript-eslint/no-unused-vars
import { useJobsQuery } from '@help-line/api/admin';

export function App() {
  const jobsQuery = useJobsQuery();
  return <>{jobsQuery.isLoading ? 'loading' : jobsQuery.data?.length}</>;
}

export default App;
