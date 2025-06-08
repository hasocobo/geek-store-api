package com.payment.PaymentService.service;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.payment.PaymentService.events.PaymentProcessed;
import com.payment.PaymentService.events.PaymentRequested;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.kafka.core.KafkaTemplate;
import org.springframework.stereotype.Service;

@Service
public class PaymentProcessor {

    @Autowired
    private KafkaTemplate<String, String> kafka;

    @Autowired
    private ObjectMapper mapper;

    public void process(PaymentRequested event) {
        System.out.println("Processing payment for order: " + event.getOrderId());

        PaymentProcessed processed = new PaymentProcessed();
        processed.setPaymentId(event.getPaymentId());
        processed.setOrderId(event.getOrderId());
        processed.setStatus("CONFIRMED");

        try {
            String json = mapper.writeValueAsString(processed);
            kafka.send("payment.processed", processed.getPaymentId(), json);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
