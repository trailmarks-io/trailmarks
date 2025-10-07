import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { WandersteinService, WandersteinResponse } from './wanderstein';
import { provideHttpClient } from '@angular/common/http';

describe('WandersteinService', () => {
  let service: WandersteinService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        WandersteinService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(WandersteinService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get recent wandersteine', () => {
    const mockData: WandersteinResponse[] = [
      {
        id: 1,
        name: 'Test Stone',
        unique_Id: 'WS-001',
        preview_Url: 'https://example.com/1.jpg',
        created_At: '2024-01-01T00:00:00Z'
      }
    ];

    service.getRecentWandersteine().subscribe(data => {
      expect(data).toEqual(mockData);
      expect(data.length).toBe(1);
    });

    const req = httpMock.expectOne('http://localhost:8080/api/wandersteine/recent');
    expect(req.request.method).toBe('GET');
    req.flush(mockData);
  });

  it('should get all wandersteine', () => {
    const mockData: WandersteinResponse[] = [
      {
        id: 1,
        name: 'Stone 1',
        unique_Id: 'WS-001',
        preview_Url: 'https://example.com/1.jpg',
        created_At: '2024-01-01T00:00:00Z'
      },
      {
        id: 2,
        name: 'Stone 2',
        unique_Id: 'WS-002',
        preview_Url: 'https://example.com/2.jpg',
        created_At: '2024-01-02T00:00:00Z'
      }
    ];

    service.getAllWandersteine().subscribe(data => {
      expect(data).toEqual(mockData);
      expect(data.length).toBe(2);
    });

    const req = httpMock.expectOne('http://localhost:8080/api/wandersteine');
    expect(req.request.method).toBe('GET');
    req.flush(mockData);
  });

  it('should handle HTTP errors', () => {
    service.getRecentWandersteine().subscribe({
      next: () => fail('should have failed'),
      error: (error) => {
        expect(error.status).toBe(500);
      }
    });

    const req = httpMock.expectOne('http://localhost:8080/api/wandersteine/recent');
    req.flush('Server error', { status: 500, statusText: 'Internal Server Error' });
  });
});
