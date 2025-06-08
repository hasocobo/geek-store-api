package com.payment.PaymentService.kafka;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.payment.PaymentService.events.PaymentRequested;
import com.payment.PaymentService.service.PaymentProcessor;
import org.apache.kafka.clients.consumer.ConsumerRecord;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.kafka.annotation.KafkaListener;
import org.springframework.stereotype.Service;

@Service
public class PaymentListener {

    @Autowired
    private ObjectMapper mapper;

    @Autowired
    private PaymentProcessor processor;

    @KafkaListener(topics = "payment.requested", groupId = "payment-service")
    public void listen(ConsumerRecord<String, String> record) {
        try {
            PaymentRequested event = mapper.readValue(record.value(), PaymentRequested.class);
            processor.process(event);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
