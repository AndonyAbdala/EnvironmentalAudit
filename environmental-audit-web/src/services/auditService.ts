export interface Audit {
  id: string;
  companyName: string;
  facilityName: string;
  responsible: string;
  startDate: string;
  endDate: string;
  status: string;
  createdAt: string;
}

const API_URL = 'https://localhost:7082/api';

export async function getAudits(): Promise<Audit[]> {
  const response = await fetch(`${API_URL}/Audits`);

  if (!response.ok) {
    throw new Error('Failed to fetch audits');
  }

  return response.json();
}