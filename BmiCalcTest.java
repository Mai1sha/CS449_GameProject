import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.assertEquals;

public class BmiCalcTest {

    @Test
    void BmiCalc_givenExample() {
        BmiCalc calculator = new BmiCalc();
        double actual = calculator.calculateBMI(65.0, 1.53); // weightKg, heightM
        assertEquals(27.77, actual, 0.01); // tolerance for floating point
    }
}

