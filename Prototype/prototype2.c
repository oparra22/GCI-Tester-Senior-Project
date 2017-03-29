//most recent code trying to figure out pin testing
/*******************************************************
This program was created by the CodeWizardAVR V3.24 
Automatic Program Generator
© Copyright 1998-2015 Pavel Haiduc, HP InfoTech s.r.l.
http://www.hpinfotech.com

Project : 
Version : 
Date    : 9/26/2016
Author  : 
Company : 
Comments:   This code is a working version of test 1. This is reading in switches which enter the values
for A0-A3. Since the timer is initialized PB1 is tied to 0 so I have ignored that pin and tied A1 to ground only bieng able to test all numbers that
do not set that bit high. I will attempt to edit this code to start creating functions and cleaning up the mess of
code we do not need



Chip type               : ATmega16
Program type            : Application
AVR Core Clock frequency: 3.680000 MHz
Memory model            : Small
External RAM size       : 0
Data Stack size         : 256
*******************************************************/

#include <io.h>
#include <main.h> //Our manually created header file
#include <delay.h>
#include <string.h>
char fail;
unsigned int pinToTest;
unsigned int odd = 0; 
int ADC_sum;
int ADC_percent;
int ADC_output;
float Voltage_actual;
float Voltage_actualB;
float pinVolt = 0;
int channelA;//Set in main
int channelB;//Set in main
int j;   //Used in main


// Declare your global variables here

// Standard Input/Output functions
#include <stdio.h>
unsigned int count;
int channelA;
int channelB;


// Voltage Reference: AREF pin
#define ADC_VREF_TYPE ((0<<REFS1) | (0<<REFS0) | (0<<ADLAR))

// Read the AD conversion result
unsigned int read_adc(unsigned char adc_input)
{
ADMUX=adc_input | ADC_VREF_TYPE;
// Delay needed for the stabilization of the ADC input voltage
delay_us(10);
// Start the AD conversion
ADCSRA|=(1<<ADSC);
// Wait for the AD conversion to complete
while ((ADCSRA & (1<<ADIF))==0);
ADCSRA|=(1<<ADIF);
return ADCW;
}

int adcAverage()
{
//int i;
//ADC_sum=0; // initialize ADC_sum;
//for (i=0;i < 10; i++){ // start to sum 10 ADC samples;
//delay_ms(10); // take ADC sample every 10 ms;
//ADC_sum += read_adc(0); // read ADC output from PA0 and add to ADC_sum;
//}

ADC_output = ADC_sum/10; // averaged ADC output;
ADC_percent = ADC_output*25/256;
// calculate percentage of ADC full range for current ADC output;

//Voltage_actual = ((ADC_sum/10)*5)/(1023.0);
Voltage_actual = (read_adc(0)*5)/1023.0;
Voltage_actualB = (read_adc(1)*5)/1023.0;
//print pin testing, pin voltage, voltage measured. 
if(!odd){ 
      pinVolt = 5 -  Voltage_actual;
      }
      else{
      pinVolt = 5 - Voltage_actualB;
      }
}
void WriteChar()
{


}

void LEDIndicator()
{
    //Show Results on LEDs 
    if(fail == 'f')
    {
        //Turn Red LED On    
        PORTC = 0xFD;
    }             
    else if(fail == 'p')
    {
        //Turn Green LED On   
        PORTC = 0xFE;
         
    }  
}

int SelectMuxChannelA()
{
//This is the Channel selector for bits A0-A3
    return (((pinToTest - 1) % 32) / 2);
}

int SelectMuxChannelB()
{
    return (((pinToTest - 1) % 32) / 2);
//This is the Channel selector for bits B0-B3
}

int SelectMuxEnable()
{
    return ((pinToTest - 1) / 16) + 1;//This will be able to stay the same.
    
}

void PrintToConsole(/*Results*/)
{
    //Print to console. What we had working before    
    
    PORTB = pinToTest;//Last addition 2/15/2017 9:03 pm Not tested yet
    
    odd = pinToTest %2; 
    if (pinVolt < 0.1){
     fail = 'f';
    }   
    else{
     fail = 'p';
    }
    if(odd)
    {     
        printf("Pin: %d Pin Voltage = %1.3f Output Voltage = %1.3f Enable: %d AChannel: %d\ status: %c\r",pinToTest,
    pinVolt, Voltage_actual, SelectMuxEnable(), SelectMuxChannelA(), fail); // Display ADC output to terminal;
              
              PORTB = SelectMuxChannelA();
    }
    else//even
    {     
          printf("Pin: %d Pin Voltage = %1.3f Output Voltage = %1.3f Enable: %d Bchannel:%d status: %c\r",pinToTest,
    pinVolt, Voltage_actualB, SelectMuxEnable(), SelectMuxChannelB(), fail); // Display ADC output to terminal;
     
         PORTB = SelectMuxChannelB() << 4;
    } 
    LEDIndicator();
    
}

void main(void)

//int pinToTest;
                                                                                                  
{
    // Declare your local variables here
    // Input/Output Ports initialization
    // Port A initialization
    // Function: Bit7=In Bit6=In Bit5=In Bit4=In Bit3=In Bit2=In Bit1=In Bit0=In 
    DDRA=(0<<DDA7) | (0<<DDA6) | (0<<DDA5) | (0<<DDA4) | (0<<DDA3) | (0<<DDA2) | (0<<DDA1) | (0<<DDA0);
    // State: Bit7=T Bit6=T Bit5=T Bit4=T Bit3=T Bit2=T Bit1=T Bit0=T 
    PORTA=(0<<PORTA7) | (0<<PORTA6) | (0<<PORTA5) | (0<<PORTA4) | (0<<PORTA3) | (0<<PORTA2) | (0<<PORTA1) | (0<<PORTA0);

    // Port B initialization
    // Function: Bit7=Out Bit6=Out Bit5=Out Bit4=Out Bit3=Out Bit2=Out Bit1=Out Bit0=Out 
    DDRB=(1<<DDB7) | (1<<DDB6) | (1<<DDB5) | (1<<DDB4) | (1<<DDB3) | (1<<DDB2) | (1<<DDB1) | (1<<DDB0);
    // State: Bit7=1 Bit6=1 Bit5=1 Bit4=1 Bit3=1 Bit2=1 Bit1=1 Bit0=1 
    PORTB=(1<<PORTB7) | (1<<PORTB6) | (1<<PORTB5) | (1<<PORTB4) | (1<<PORTB3) | (1<<PORTB2) | (1<<PORTB1) | (1<<PORTB0);

    // Port C initialization
    // Function: Bit7=Out Bit6=Out Bit5=Out Bit4=Out Bit3=Out Bit2=Out Bit1=Out Bit0=Out 
    DDRC=(1<<DDC7) | (1<<DDC6) | (1<<DDC5) | (1<<DDC4) | (1<<DDC3) | (1<<DDC2) | (1<<DDC1) | (1<<DDC0);
    // State: Bit7=T Bit6=T Bit5=T Bit4=T Bit3=T Bit2=T Bit1=T Bit0=T 
    PORTC=(1<<PORTC7) | (1<<PORTC6) | (1<<PORTC5) | (1<<PORTC4) | (1<<PORTC3) | (1<<PORTC2) | (1<<PORTC1) | (1<<PORTC0);

    // Port D initialization
    // Function: Bit7=In Bit6=In Bit5=In Bit4=In Bit3=In Bit2=In Bit1=In Bit0=In 
    DDRD=(0<<DDD7) | (0<<DDD6) | (0<<DDD5) | (0<<DDD4) | (0<<DDD3) | (0<<DDD2) | (0<<DDD1) | (0<<DDD0);
    // State: Bit7=T Bit6=T Bit5=T Bit4=T Bit3=T Bit2=T Bit1=T Bit0=T 
    PORTD=(0<<PORTD7) | (0<<PORTD6) | (0<<PORTD5) | (0<<PORTD4) | (0<<PORTD3) | (0<<PORTD2) | (0<<PORTD1) | (0<<PORTD0);

    // Timer/Counter 0 initialization
    // Clock source: System Clock
    // Clock value: Timer 0 Stopped
    // Mode: Normal top=0xFF
    // OC0 output: Disconnected
    TCCR0=(0<<WGM00) | (0<<COM01) | (0<<COM00) | (0<<WGM01) | (0<<CS02) | (0<<CS01) | (0<<CS00);
    TCNT0=0x00;
    OCR0=0x00;


    // Timer(s)/Counter(s) Interrupt(s) initialization
    TIMSK=(0<<OCIE2) | (0<<TOIE2) | (0<<TICIE1) | (0<<OCIE1A) | (0<<OCIE1B) | (0<<TOIE1) | (0<<OCIE0) | (0<<TOIE0);

    // External Interrupt(s) initialization
    // INT0: Off
    // INT1: Off
    // INT2: Off
    MCUCR=(0<<ISC11) | (0<<ISC10) | (0<<ISC01) | (0<<ISC00);
    MCUCSR=(0<<ISC2);

    // USART initialization
    // Communication Parameters: 8 Data, 1 Stop, No Parity
    // USART Receiver: On
    // USART Transmitter: On          
    // USART Mode: Asynchronous
    // USART Baud Rate: 9600
    UCSRA=(0<<RXC) | (0<<TXC) | (0<<UDRE) | (0<<FE) | (0<<DOR) | (0<<UPE) | (0<<U2X) | (0<<MPCM);
    UCSRB=(0<<RXCIE) | (0<<TXCIE) | (0<<UDRIE) | (1<<RXEN) | (1<<TXEN) | (0<<UCSZ2) | (0<<RXB8) | (0<<TXB8);
    UCSRC=(1<<URSEL) | (0<<UMSEL) | (0<<UPM1) | (0<<UPM0) | (0<<USBS) | (1<<UCSZ1) | (1<<UCSZ0) | (0<<UCPOL);
    UBRRH=0x00;
    UBRRL=0x17;

    // Analog Comparator initialization
    // Analog Comparator: Off
    // The Analog Comparator's positive input is
    // connected to the AIN0 pin
    // The Analog Comparator's negative input is
    // connected to the AIN1 pin
    ACSR=(1<<ACD) | (0<<ACBG) | (0<<ACO) | (0<<ACI) | (0<<ACIE) | (0<<ACIC) | (0<<ACIS1) | (0<<ACIS0);

    // ADC initialization
    // ADC Clock frequency: 920.000 kHz
    // ADC Voltage Reference: AREF pin
    // ADC Auto Trigger Source: Free Running
    ADMUX=ADC_VREF_TYPE;
    ADCSRA=(1<<ADEN) | (0<<ADSC) | (1<<ADATE) | (0<<ADIF) | (0<<ADIE) | (0<<ADPS2) | (1<<ADPS1) | (0<<ADPS0);
    SFIOR=(0<<ADTS2) | (0<<ADTS1) | (0<<ADTS0);

    // SPI initialization
    // SPI disabled
    SPCR=(0<<SPIE) | (0<<SPE) | (0<<DORD) | (0<<MSTR) | (0<<CPOL) | (0<<CPHA) | (0<<SPR1) | (0<<SPR0);

    // TWI initialization
    // TWI disabled
    TWCR=(0<<TWEA) | (0<<TWSTA) | (0<<TWSTO) | (0<<TWEN) | (0<<TWIE);

    // Globally enable interrupts
    //#asm("sei")

    while (1)
    {      
        //Testing loop through pin numbers
        for(j = 1; j < 34;j++)
        {
            //set pinTotest as variable J
            pinToTest = j;   
            adcAverage();
            PrintToConsole();
            delay_ms(1000);
        }
    }
}

               
