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

export interface CreateAuditRequest {
  companyName: string;
  facilityName: string;
  responsible: string;
  startDate: string;
  endDate: string;
}

export interface CalculateAuditRequest {
  electricityKwh: number;
  naturalGasM3: number;

  waterConsumptionM3: number;
  wastewaterM3: number;

  hazardousWasteKg: number;
  nonHazardousWasteKg: number;
  recycledWasteKg: number;

  dieselLiters: number;
  gasolineLiters: number;
}

export interface AuditCalculationResult {
  score: number;
  totalEmissions: number;
  recyclingRate: number;
  totalWaste: number;
}

const API_URL = "https://localhost:7082/api";

export async function getAudits(): Promise<Audit[]> {
  const response = await fetch(`${API_URL}/Audits`);

  if (!response.ok) {
    throw new Error("Failed to fetch audits");
  }

  return response.json();
}

export async function getAudit(id: string): Promise<Audit> {
  const response = await fetch(`${API_URL}/Audits/${id}`);

  if (!response.ok) {
    throw new Error("Failed to fetch audit");
  }

  return response.json();
}

export async function createAudit(request: CreateAuditRequest): Promise<Audit> {
  const response = await fetch(`${API_URL}/Audits`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to create audit");
  }

  return response.json();
}

export async function calculateAudit(
  id: string,
  request: CalculateAuditRequest,
): Promise<AuditCalculationResult> {
  const response = await fetch(`${API_URL}/Audits/${id}/calculate`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to calculate audit");
  }

  return response.json();
}

export async function downloadAuditReport(id: string): Promise<Blob> {
  const response = await fetch(`${API_URL}/Audits/${id}/report`);

  if (!response.ok) {
    throw new Error("Failed to download audit report");
  }

  return response.blob();
}
