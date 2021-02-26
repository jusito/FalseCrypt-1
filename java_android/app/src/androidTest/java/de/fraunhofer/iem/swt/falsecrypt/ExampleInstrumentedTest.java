package de.fraunhofer.iem.swt.falsecrypt;

import android.os.Environment;

import androidx.test.ext.junit.runners.AndroidJUnit4;

import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;

import org.junit.Rule;
import androidx.test.ext.junit.rules.ActivityScenarioRule;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;

import static androidx.test.espresso.Espresso.onView;
import static androidx.test.espresso.action.ViewActions.click;
import static androidx.test.espresso.assertion.ViewAssertions.matches;
import static androidx.test.espresso.matcher.ViewMatchers.isCompletelyDisplayed;
import static androidx.test.espresso.matcher.ViewMatchers.withId;

import static org.junit.Assert.*;

/**
 * Instrumented test, which will execute on an Android device.
 *
 * @see <a href="http://d.android.com/tools/testing">Testing documentation</a>
 */
@RunWith(AndroidJUnit4.class)
public class ExampleInstrumentedTest {
    private final File testfile = new File(Environment.getExternalStorageDirectory(), "TestFolder/testfile.txt");
    private final File testfileEncrypted = new File(testfile.getAbsoluteFile().getPath() + ".falsecrypt");

    @Rule public ActivityScenarioRule<MainActivity> activityScenarioRule
            = new ActivityScenarioRule<>(MainActivity.class);

    @Before
    public void browseFirst() {
        onView(withId(R.id.browse)).check(matches(isCompletelyDisplayed()));
        onView(withId(R.id.browse))
                .perform(click());

        onView(withId(R.id.testfiles))
                .perform(click());

        assertTrue(testfile.exists());
    }

    @Test
    public void checkEncryptionDecryption() throws IOException {
        // save file content
        final String content = new String(Files.readAllBytes(testfile.toPath()));

        // encrypt file
        onView(withId(R.id.encrypt)).perform(click());

        // check encrypted file
        assertNotEquals(content, new String(Files.readAllBytes(testfileEncrypted.toPath())));

        // decrypt file
        onView(withId(R.id.decrypt)).perform(click());

        // check decryption
        assertEquals(content, new String(Files.readAllBytes(testfile.toPath())));
    }
}
