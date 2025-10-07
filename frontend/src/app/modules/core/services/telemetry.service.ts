import { Injectable } from '@angular/core';
import { WebTracerProvider } from '@opentelemetry/sdk-trace-web';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { SimpleSpanProcessor } from '@opentelemetry/sdk-trace-web';
import { ZoneContextManager } from '@opentelemetry/context-zone';
import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { FetchInstrumentation } from '@opentelemetry/instrumentation-fetch';
import { XMLHttpRequestInstrumentation } from '@opentelemetry/instrumentation-xml-http-request';
import { DocumentLoadInstrumentation } from '@opentelemetry/instrumentation-document-load';
import { Resource } from '@opentelemetry/resources';

@Injectable({
  providedIn: 'root'
})
export class TelemetryService {
  private provider?: WebTracerProvider;

  initializeTracing(endpoint: string = 'http://localhost:4318/v1/traces'): void {
    const resource = Resource.default().merge(
      new Resource({
        'service.name': 'trailmarks-frontend',
        'service.version': '1.0.0'
      })
    );

    this.provider = new WebTracerProvider({
      resource: resource
    });

    // Configure OTLP exporter
    const exporter = new OTLPTraceExporter({
      url: endpoint
    });

    this.provider.addSpanProcessor(new SimpleSpanProcessor(exporter));

    // Register the provider globally
    this.provider.register({
      contextManager: new ZoneContextManager()
    });

    // Register instrumentations
    registerInstrumentations({
      instrumentations: [
        new FetchInstrumentation({
          propagateTraceHeaderCorsUrls: [
            /http:\/\/localhost:8080\/.*/,
            /http:\/\/backend:8080\/.*/
          ],
          clearTimingResources: true
        }),
        new XMLHttpRequestInstrumentation({
          propagateTraceHeaderCorsUrls: [
            /http:\/\/localhost:8080\/.*/,
            /http:\/\/backend:8080\/.*/
          ]
        }),
        new DocumentLoadInstrumentation()
      ]
    });

    console.log('OpenTelemetry initialized with endpoint:', endpoint);
  }

  shutdown(): void {
    if (this.provider) {
      this.provider.shutdown();
    }
  }
}
